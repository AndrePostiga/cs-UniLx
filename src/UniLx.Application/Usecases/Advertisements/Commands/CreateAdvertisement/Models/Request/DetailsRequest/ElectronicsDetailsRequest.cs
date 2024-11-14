﻿namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request.DetailsRequest
{
    public class ElectronicsDetailsRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? ProductType { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? StorageCapacity { get; set; }
        public string? Memory { get; set; }
        public string? Processor { get; set; }
        public string? GraphicsCard { get; set; }
        public float? BatteryLife { get; set; }
        public DateTime? WarrantyUntil { get; set; }
        public List<string>? Features { get; set; }
        public string? Condition { get; set; }
        public bool? IncludesOriginalBox { get; set; }
        public List<string>? Accessories { get; set; }
    }
}