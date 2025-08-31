using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEN_Yoga.Models;
using ZEN_Yoga.Services.Interfaces.Payment;

namespace ZEN_Yoga.Services.Services.Payment
{

    public class PaymentService : IPaymentService
    {
        private readonly ZenYogaDbContext _dbContext;
        public PaymentService(ZenYogaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddPayment(int userId, int studioId, int subscriptionId)
        {
            var subscription = await _dbContext.StudioSubscriptions.FirstOrDefaultAsync(ss => ss.StudioId == studioId && ss.SubscriptionTypeId == subscriptionId);

            if (subscription == null)
            {
                return false;
            }

            var payment = new Models.Payment()
            {
                UserId = userId,
                StudioId = studioId,
                SubscriptionTypeId = subscriptionId,
                CreatedAt = DateTime.Now,
                PaymentDate = DateTime.Now,
                Amount = subscription.Price,
                Status = "procsessing"
            };

            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
