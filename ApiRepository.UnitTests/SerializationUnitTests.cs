namespace ApiRepository.UnitTests
{
    using LocationData.ApiRepository;
    using LocationData.Core.Models.City;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    [TestClass]
    public class SerializationUnitTests
    {
        private Serialization _serialization;

        [TestInitialize]
        public void TestInitialize()
        {
            _serialization = new Serialization(new JsonSerializer());
        }

        #region Constructor tests

        [TestMethod]
        public void Serialization_JsonSerializerNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => { _serialization = new Serialization(null); });

        }

        #endregion

        #region Method tests

        [TestMethod]
        public async Task Deserialize_HttpResponseMessageNull_ThrowsArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(
                async () => { await _serialization.Deserialize<City>(null); });
        }

        [TestMethod]
        public async Task Deserialize_ValidCallForCity_ReturnsCityObjectType()
        {
            var message = new HttpResponseMessage
            {
                Content = new StringContent(
                    "{'name':'Estonia','topLevelDomain':['.ee'],'alpha2Code':'EE','alpha3Code':'EST','callingCodes':['372'],'capital':'Tallinn','altSpellings':['EE','Eesti','Republic of Estonia','Eesti Vabariik'],'region':'Europe','subregion':'Northern Europe','population':1315944,'latlng':[59.0,26.0],'demonym':'Estonian','area':45227.0,'gini':36.0,'timezones':['UTC+02:00'],'borders':['LVA','RUS'],'nativeName':'Eesti','numericCode':'233','currencies':[{'code':'EUR','name':'Euro','symbol':'€'}],'languages':[{'iso639_1':'et','iso639_2':'est','name':'Estonian','nativeName':'eesti'}],'translations':{'de':'Estland','es':'Estonia','fr':'Estonie','ja':'エストニア','it':'Estonia','br':'Estônia','pt':'Estónia','nl':'Estland','hr':'Estonija','fa':'استونی'},'flag':'https://restcountries.eu/data/est.svg','regionalBlocs':[{'acronym':'EU','name':'European Union','otherAcronyms':[],'otherNames':[]}],'cioc':'EST'}")
            };
            var response = await _serialization.Deserialize<City>(message);
            response.ShouldBeOfType<City>();
        }

        #endregion
    }
}
