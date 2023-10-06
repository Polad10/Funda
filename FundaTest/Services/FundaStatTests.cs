using Funda.Enums;
using Funda.Models;
using Funda.Services;
using Funda.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundaTest.Services
{
    public class FundaStatTests
    {
        private readonly Mock<IFundaApi> _fundaApiMock;

        private readonly FundaStat _fundaStat;

        public FundaStatTests()
        {
            _fundaApiMock = new Mock<IFundaApi>();
            _fundaStat = new FundaStat(_fundaApiMock.Object);
        }

        [Fact]
        public async Task GetTopAgents_CorrectlySortsTopAgents()
        {
            var city = "eindhoven";
            var type = FundaObjectType.Buy;
            var withGarden = false;

            var saleObject1 = new SaleObject() { AgentName = "Agent1" };
            var saleObject2 = new SaleObject() { AgentName = "Agent2" };
            var saleObject3 = new SaleObject() { AgentName = "Agent3" };
            var saleObject4 = new SaleObject() { AgentName = "Agent4" };

            var saleObjects = new List<SaleObject>() 
            { 
                saleObject1, saleObject1, saleObject1, saleObject1, 
                saleObject2, saleObject2, saleObject2, 
                saleObject3, saleObject3, 
                saleObject4 
            };

            _fundaApiMock.Setup(a => a.GetSaleObjects(city, type, withGarden, CancellationToken.None, null)).ReturnsAsync(saleObjects);

            var topAgents = await _fundaStat.GetTopAgents(city, type, withGarden, CancellationToken.None, null, 10);
            var keyValues = topAgents.ToList();

            Assert.Equal(4, topAgents.Count);

            Assert.Equal(saleObject1.AgentName, keyValues[0].Key);
            Assert.Equal(4, keyValues[0].Value);

            Assert.Equal(saleObject2.AgentName, keyValues[1].Key);
            Assert.Equal(3, keyValues[1].Value);

            Assert.Equal(saleObject3.AgentName, keyValues[2].Key);
            Assert.Equal(2, keyValues[2].Value);

            Assert.Equal(saleObject4.AgentName, keyValues[3].Key);
            Assert.Equal(1, keyValues[3].Value);
        }
    }
}
