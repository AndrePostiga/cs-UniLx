﻿namespace UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse
{
    public class JobOpportunitiesDetailsResponse : BaseDetailsResponse
    {
        public string Position { get; set; }
        public string Company { get; set; }
        public int? Salary { get; set; }
        public bool IsSalaryDisclosed { get; set; }
        public string WorkLocation { get; set; }
        public string EmploymentType { get; set; }
        public string? ExperienceLevel { get; set; }
        public List<string>? Skills { get; set; }
        public List<string>? Benefits { get; set; }
        public bool RelocationHelp { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public ContactInformationResponse ContactInfo { get; set; }
    }

}