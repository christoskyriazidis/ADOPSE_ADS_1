using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using WpfClientt.model.jsonConverters;

namespace WpfClientt.services {
    /// <summary>
    /// Provides static methods for retrieving core services of the application.
    /// </summary>
    public class FactoryServices {
        private object lockObject = new object();

        private ICustomerService customerService;
        private IAdDetailsService adDetailsService;
        private IChatService chatService;
        private JsonSerializerOptions options;
        private OpenIdConnectClient openIdConnectClient;
        private IAdService adService;
        private HttpClient client;
        private ICustomerNotifier notifier;

        public FactoryServices() {
            client = new HttpClient();
            customerService = new CustomerServiceImpl(client);
            adDetailsService = new AdDetailsServiceImpl(client);
            notifier = new ToastCustomerNotifier();
        }

        public ICustomerNotifier CustomerNotifier() {
            return notifier;
        }

        public async Task<IAdService> AdServiceInstance() {
            CategoryConverter categoryConverter = await CategoryConverter.getInstance(adDetailsService);
            ConditionConverter conditionConverter = await ConditionConverter.getInstance(adDetailsService);
            ManufacturerConverter manufacturerConverter = await ManufacturerConverter.getInstance(adDetailsService);
            StateConverter stateConverter = await StateConverter.getInstance(adDetailsService);
            SubcategoryConverter subcategoryConverter = await SubcategoryConverter.getInstance(adDetailsService);
            TypeConverter typeConverter = await TypeConverter.getInstance(adDetailsService);
            if(adService == null) {
                lock (lockObject) {
                    if (options == null) {
                        options = new JsonSerializerOptions();
                        options.Converters.Add(categoryConverter);
                        options.Converters.Add(conditionConverter);
                        options.Converters.Add(manufacturerConverter);
                        options.Converters.Add(stateConverter);
                        options.Converters.Add(subcategoryConverter);
                        options.Converters.Add(typeConverter);
                    }
                }
                adService = new AdServiceImpl(client, options);
            }
            
            return adService;
        }

        public async Task<IChatService> ChatServiceInstance() {
            CategoryConverter categoryConverter = await CategoryConverter.getInstance(adDetailsService);
            ConditionConverter conditionConverter = await ConditionConverter.getInstance(adDetailsService);
            ManufacturerConverter manufacturerConverter = await ManufacturerConverter.getInstance(adDetailsService);
            StateConverter stateConverter = await StateConverter.getInstance(adDetailsService);
            SubcategoryConverter subcategoryConverter = await SubcategoryConverter.getInstance(adDetailsService);
            TypeConverter typeConverter = await TypeConverter.getInstance(adDetailsService);

            if(chatService == null) {
                lock (lockObject) {
                    if(options == null) {
                        options = new JsonSerializerOptions();
                        options.Converters.Add(categoryConverter);
                        options.Converters.Add(conditionConverter);
                        options.Converters.Add(manufacturerConverter);
                        options.Converters.Add(stateConverter);
                        options.Converters.Add(subcategoryConverter);
                        options.Converters.Add(typeConverter);
                    }
                }
                chatService = await ChatServiceSignalR.GetInstance(client,options,await AdServiceInstance(),CustomerServiceInstance());
            }

            return chatService;
        }

        public ICustomerService CustomerServiceInstance() {
            return customerService;
        }

        public IAdDetailsService AdDetailsServiceInstance() {
            return adDetailsService;
        }

        public async Task<OpenIdConnectClient> GetOpenIdConnectClient() {
            if(openIdConnectClient == null) {
                DiscoveryDocumentResponse discovery = await client.GetDiscoveryDocumentAsync("https://localhost:44305");
                if (discovery.IsError) {
                    throw new ApplicationException("Couldn't obtaing discovery information about the identity server.");
                }
                openIdConnectClient = new OpenIdConnectClient(client,discovery);
            }
            return openIdConnectClient;
        }

    }
}
