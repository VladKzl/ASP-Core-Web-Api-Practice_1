﻿using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using Microsoft.Net.Http.Headers;
using System.Dynamic;

namespace CompanyEmployees.Utility
{
    public class EmployeeLinks : IEmployeeLinks
    {
        public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<EmployeeDto> _dataShaper;

        public LinkResponse TryGenerateLinks(
        IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext)
        {
            var shapedEmployees = ShapeData(employeesDto, fields);
            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedEmployees(employeesDto, fields, companyId, httpContext,
                shapedEmployees);
            return ReturnShapedEmployees(shapedEmployees);
        }
        private List<ExpandoObject> ShapeData(IEnumerable<EmployeeDto> employeesDto, string fields)
        =>
        _dataShaper.ShapeData(employeesDto, fields)
        .Select(e => e.Entity)
        .ToList();
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",
            StringComparison.InvariantCultureIgnoreCase);
        }
        private LinkResponse ReturnShapedEmployees(List<ExpandoObject> shapedEmployees) =>
        new LinkResponse { ShapedEntities = shapedEmployees };
        private LinkResponse ReturnLinkdedEmployees(IEnumerable<EmployeeDto> employeesDto,
        string fields, Guid companyId, HttpContext httpContext, List<ExpandoObject> shapedEmployees)
        {
            var employeeDtoList = employeesDto.ToList();
            for (var index = 0; index < employeeDtoList.Count(); index++)
            {
                var employeeLinks = CreateLinksForEmployee(httpContext, companyId,
                employeeDtoList[index].Id, fields);
                shapedEmployees[index].TryAdd("Links", employeeLinks); // Изменил Entity на ExpandoObject
            }
            var employeeCollection = new LinkCollectionWrapper<ExpandoObject>(shapedEmployees);
            var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);
            return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
        }
        private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid companyId, Guid id, string fields = "")
        {
            var links = new List<Link>
            {
            new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany",
            values: new { companyId, id, fields }),
            "self",
            "GET"),
            new Link(_linkGenerator.GetUriByAction(httpContext,
            "DeleteEmployeeForCompany", values: new { companyId, id }),
            "delete_employee",
            "DELETE"),
            new Link(_linkGenerator.GetUriByAction(httpContext,
            "UpdateEmployeeForCompany", values: new { companyId, id }),
            "update_employee",
            "PUT"),
            new Link(_linkGenerator.GetUriByAction(httpContext,
            "PartiallyUpdateEmployeeForCompany", values: new { companyId, id }),
            "partially_update_employee",
            "PATCH")
            };
            return links;
        }
        private LinkCollectionWrapper<ExpandoObject> CreateLinksForEmployees(
        HttpContext httpContext, LinkCollectionWrapper<ExpandoObject> employeesWrapper)
        {
            employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext,
            "GetEmployeesForCompany", values: new { }),"self","GET"));
            return employeesWrapper;
        }
    }
}
