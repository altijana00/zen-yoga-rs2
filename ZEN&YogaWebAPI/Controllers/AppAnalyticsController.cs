using Microsoft.AspNetCore.Mvc;
using ZEN_Yoga.Services.Interfaces.Analytics;

namespace ZEN_YogaWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AppAnalyticsController : ControllerBase
    {
        private readonly IAppAnalyticsService _appAnalyticsService;

        public AppAnalyticsController(IAppAnalyticsService appAnalyticsService)
        {
            _appAnalyticsService = appAnalyticsService;
        }
    }
}
