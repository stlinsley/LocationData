namespace LocationData.Core.UnitTests
{
    using System.Runtime.InteropServices;
    using AutoMapper;
    using Extensions;
    using Mappings;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Models.City;
    using Moq;
    using Shouldly;

    [TestClass]
    public class AutomapperExtensionsUnitTests
    {
        private IMapper _mapper;
        private MapperConfiguration _config;
        private Mock<City> _city;

        [TestInitialize]
        public void TestInitialize()
        {
            _config = new MapperConfiguration(x => x.AddProfile(new LocationProfile()));
            _mapper = new Mapper(_config);
            _city = new Mock<City>();
        }

        [TestMethod]
        public void Map_MapsCityToCombined()
        {
            var result =_mapper.Map<CombinedLocationData>(_city.Object);
            result.ShouldBeOfType<CombinedLocationData>();
        }
    }
}
