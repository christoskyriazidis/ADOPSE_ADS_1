using ApiOne.Models;
using ApiOne.Models.Customer;
using ApiOne.Models.Queries;
using ApiOne.Models.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Interfaces
{
    public interface ICustomerRepository
    {
        CustomersWithPagination GetCustomers(Pagination adParameters);
        CustomerDetails GetCustomer(int id);
        CustomerDetails GetMyProfileInfo(int id);

        bool CheckIfCustomerOwnThisAd(int AdId,int CustomerId);

        bool UpdateProfile(CustomerDetails customerDetails);
        bool UpdateProfileImage(int customerId);
        public int GetCustomerIdFromSub(string SubId);
        bool ReviewAndRateCustomer(PostReview postReview);
        bool SellAd(int AdId,int BuyerId);


    }
}
