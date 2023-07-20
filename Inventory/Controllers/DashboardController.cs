﻿using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            var model = _dashboardService.GaugeGrade();
            return View(model);
        }
    }
}
