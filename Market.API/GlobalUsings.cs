// Global using statements for the entire application
// This eliminates the need for repetitive using statements in every file

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;

// Logging
global using Microsoft.Extensions.Logging;

// Dependency Injection
global using Microsoft.Extensions.DependencyInjection;

// Models and Entities
global using Market.API.Common;
global using Market.API.Models;
global using Market.API.Models.Entities;
global using Market.API.Models.Enums;
global using Market.API.Models.ValueObjects;

// Data Access
global using Market.API.Data;
global using Market.API.Data.Configurations;
global using Market.API.Data.Interfaces;
global using Market.API.Data.Repositories;
global using Market.API.Data.UnitOfWork;
global using Market.API.Data.Seeds;

// Middleware
global using Market.API.Middleware;

// Settings
global using Market.API.Settings;

// AspNetCore
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Mvc;
