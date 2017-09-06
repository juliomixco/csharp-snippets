using ChargifyNET;
using TestProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.clases
{
    public class ChargifyTools
    {
               
        public static ChargifyConnect Chargify
        {
            get
            {
                ChargifyAccountRetrieverSection config = (ChargifyAccountRetrieverSection)ConfigurationManager.GetSection("chargify");
                ChargifyAccountElement accountInfo = config.GetDefaultOrFirst();
                ChargifyConnect chargifyConnect = new ChargifyConnect();
                chargifyConnect.apiKey = accountInfo.ApiKey;
                chargifyConnect.Password = accountInfo.ApiPassword;
                chargifyConnect.URL = accountInfo.Site;
                chargifyConnect.SharedKey = accountInfo.SharedKey;
                //chargifyConnect.ProtocolType = SecurityProtocolType.Tls11;
                chargifyConnect.ProtocolType = System.Net.SecurityProtocolType.Tls12;
                return chargifyConnect;
            }
        }

        public static IProduct getChargifyProduct(string productHandle)
        {
            IProduct result = (from a in Chargify.GetProductList().Values
                               where a.Handle == productHandle
                               select a).FirstOrDefault();
            return result;
        }
        public static bool IsChargifyProduct(string productHandle)
        {
            var result = (from a in Chargify.GetProductList().Values
                          where a.Handle == productHandle
                          select a);
            if (result != null || result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static bool allowLogin(string billingRefNumber)
        {
            ICustomer customer = (from a in Chargify.GetCustomerList().Values
                                  where a.SystemID == billingRefNumber
                                  select a).FirstOrDefault();
            if (customer==null)
            {
                return false;
            }
            ISubscription subscription = (from s in Chargify.GetSubscriptionListForCustomer(customer.ChargifyID).Values
                                          select s).FirstOrDefault();
            if (subscription != null && (subscription.State== SubscriptionState.Active|| subscription.State == SubscriptionState.Trialing))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static bool allowLogin(ISubscription subscription)
        {
            
            if (subscription != null && (subscription.State == SubscriptionState.Active || subscription.State == SubscriptionState.Trialing))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static ISubscription getSubscription(string billingRefNumber)
        {
            ICustomer customer = (from a in Chargify.GetCustomerList().Values
                                  where a.SystemID == billingRefNumber
                                  select a).FirstOrDefault();
            if (customer == null)
            {
                return null;
            }
            ISubscription subscription = (from s in Chargify.GetSubscriptionListForCustomer(customer.ChargifyID).Values
                                          select s).FirstOrDefault();

            return subscription;
        }
        public static bool RequireCreditCard(string productHandle)
        {
            IProduct result = (from a in Chargify.GetProductList().Values
                               where a.Handle == productHandle
                               select a).FirstOrDefault();
            if (result != null)
            {
                return result.RequireCreditCard;
            }
            else
            {
                throw new ArgumentException("Not a valid product");
            }
        }
        public static bool RequireBillingAddress(string productHandle)
        {
            IProduct result = (from a in Chargify.GetProductList().Values
                               where a.Handle == productHandle
                               select a).FirstOrDefault();
            if (result != null)
            {
                /* TODO: There's currently no require_billing_address, or request_billing_address field in API->Product
                     so for now, just always request it.
                */
                return true;
            }
            else
            {
                throw new ArgumentException("Not a valid product");
            }
        }


    }
}