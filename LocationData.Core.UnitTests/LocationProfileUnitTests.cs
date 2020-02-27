namespace LocationData.Core.UnitTests
{
    using AutoMapper;
    using Mappings;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LocationProfileUnitTests
    {
        private Mapper _mapper;
        private MapperConfiguration _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _config = new MapperConfiguration(x => x.AddProfile(new LocationProfile()));
            _mapper = new Mapper(_config);
        }

        [TestMethod]
        public void LocationProfile_MapsCorrectly()
        {
           _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
