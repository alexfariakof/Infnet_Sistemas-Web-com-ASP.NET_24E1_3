﻿using Application.Account;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.CommonInjectDependence;
public class ServiceInjectDependenceTest
{
    [Fact]
    public void AddServices_Should_Register_Services()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddServices();

        // Assert
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(CustomerService)));
        Assert.NotNull(services.Any(descriptor => descriptor.ServiceType == typeof(MerchantService)));
    }
}