using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace StApp
{
    public class Contract
    {
        public string Id { get; set; }
        public string FactionSymbol { get; set; }
        public string Type { get; set; }
        public Terms Terms { get; set; }
        public bool Accepted { get; set; }
        public bool Fulfilled { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime DeadlineToAccept { get; set; }

        public void PrintToConsole()
        {
            Console.WriteLine("Id: " + Id);
            Console.WriteLine("FactionSymbol: " + FactionSymbol);
            Console.WriteLine("Type: " + Type);
            Console.WriteLine("Terms - Deadline: " + Terms.Deadline);
            Console.WriteLine("Terms - Payment - OnAccepted: " + Terms.Payment.OnAccepted);
            Console.WriteLine("Terms - Payment - OnFulfilled: " + Terms.Payment.OnFulfilled);
            Console.WriteLine("Terms - Deliver - TradeSymbol: " + Terms.Deliver.TradeSymbol);
            Console.WriteLine("Terms - Deliver - DestinationSymbol: " + Terms.Deliver.DestinationSymbol);
            Console.WriteLine("Terms - Deliver - UnitsRequired: " + Terms.Deliver.UnitsRequired);
            Console.WriteLine("Terms - Deliver - UnitsFulfilled: " + Terms.Deliver.UnitsFulfilled);
            Console.WriteLine("Accepted: " + Accepted);
            Console.WriteLine("Fulfilled: " + Fulfilled);
            Console.WriteLine("Expiration: " + Expiration);
            Console.WriteLine("DeadlineToAccept: " + DeadlineToAccept);
        }

        public async Task AcceptContract(string token)
        {
            Console.WriteLine("Do you want to accept this contract? (y/n)");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                var client = new HttpClient();
                var content = new StringContent("", Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync($"https://api.spacetraders.io/v2/my/contracts/{Id}/accept", content);
                var contract = Contract.FromJson(await response.Content.ReadAsStringAsync());
                contract.PrintToConsole();
            }
            else
            {
                Console.WriteLine("Contract not accepted.");
            }
        }

        public static Contract FromJson(string jsonString)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            var data = jsonObject.data[0];

            return new Contract
            {
                Id = data.id,
                FactionSymbol = data.factionSymbol,
                Type = data.type,
                Terms = new Terms
                {
                    Deadline = DateTime.ParseExact((string)data.terms.deadline, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                    Payment = new Payment
                    {
                        OnAccepted = (int)data.terms.payment.onAccepted,
                        OnFulfilled = (int)data.terms.payment.onFulfilled
                    },
                    Deliver = new Deliver
                    {
                        TradeSymbol = (string)data.terms.deliver[0].tradeSymbol,
                        DestinationSymbol = (string)data.terms.deliver[0].destinationSymbol,
                        UnitsRequired = (int)data.terms.deliver[0].unitsRequired,
                        UnitsFulfilled = (int)data.terms.deliver[0].unitsFulfilled
                    }
                },
                Accepted = (bool)data.accepted,
                Fulfilled = (bool)data.fulfilled,
                Expiration = DateTime.ParseExact((string)data.expiration, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                DeadlineToAccept = DateTime.ParseExact((string)data.deadlineToAccept, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            };
        }

        public static async Task<Contract> ShowContract(string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            try
            {
                var response = await client.GetAsync("https://api.spacetraders.io/v2/my/contracts");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var contract = FromJson(responseBody); // Parse the response to a Contract object
                contract.PrintToConsole();
                return contract;
            }
            catch (HttpRequestException e)
            {                
                Console.WriteLine("ErrorContext:" + e);
                return null;
            }
        }  
    }

    public class Terms
    {
        public DateTime Deadline { get; set; }
        public Payment Payment { get; set; }
        public Deliver Deliver { get; set; }
    }

    public class Payment
    {
        public int OnAccepted { get; set; }
        public int OnFulfilled { get; set; }
    }

    public class Deliver
    {
        public string TradeSymbol { get; set; }
        public string DestinationSymbol { get; set; }
        public int UnitsRequired { get; set; }
        public int UnitsFulfilled { get; set; }
    }
}