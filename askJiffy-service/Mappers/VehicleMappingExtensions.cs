﻿using askJiffy_service.Models.DTOs;
using askJiffy_service.Models.Requests;
using askJiffy_service.Models.Responses.User;

namespace askJiffy_service.Mappers
{
    public static class VehicleMappingExtensions
    {

        public static Vehicle MapToUserVehicle(this VehicleDTO vehicleDTO)
        {
            return new Vehicle
            {
                Id = vehicleDTO.Id,
                Make = vehicleDTO.Make,
                Model = vehicleDTO.Model,
                Trim = vehicleDTO.Trim,
                Year = vehicleDTO.Year,
                Chassis = vehicleDTO.Chassis,
                Transmission = vehicleDTO.Transmission,
                Mileage = vehicleDTO.Mileage
            };
        }

        // Used only for creating new VehicleDTOs; EF generates Id
        public static VehicleDTO MapToNewVehicleDTO(this SaveVehicleRequest newVehicle, UserDTO userDTO)
        {
            return new VehicleDTO
            {
                Make = newVehicle.Make,
                Model = newVehicle.Model,
                Year = newVehicle.Year,
                Trim = newVehicle.Trim,
                Chassis = newVehicle.Chassis,
                Transmission = newVehicle.Transmission,
                Mileage = newVehicle.Mileage,
                User = userDTO
            };
        }

        // used only for updating existing vehicles (meaning you don't have to pass in a userDTO)
        public static VehicleDTO MapToVehicleDTO(this SaveVehicleRequest updateVehicle, VehicleDTO existingVehicle) 
        {
            existingVehicle.Make = updateVehicle.Make;
            existingVehicle.Model = updateVehicle.Model;
            existingVehicle.Year = updateVehicle.Year;
            existingVehicle.Trim = updateVehicle.Trim;
            existingVehicle.Chassis = updateVehicle.Chassis;
            existingVehicle.Transmission = updateVehicle.Transmission;
            existingVehicle.Mileage = updateVehicle.Mileage;

            return existingVehicle;
        }
    }
}
