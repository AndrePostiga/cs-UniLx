﻿namespace UniLx.Application.Usecases.Advertisements.SharedModels.Requests
{
    public class PhoneRequest
    {
        public string CountryCode { get; set; } = "55";
        public string AreaCode { get; set; }
        public string Number { get; set; }
    }
}
