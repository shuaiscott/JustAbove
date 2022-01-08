using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JustAbove.Services;
using JustAbove.Utilities;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JustAbove.Tests
{
    public class OverheadFlightServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var flights = await OverheadFlightService.GetOverheadFlights();
        }

        [Test]
        public void OpenSkyStateJsonConverter_Maps_To_Object_Successfully()
        {
            var json = "{\"time\":1641620082,\"states\":[[\"a8d249\",\"DAL670  \",\"United States\",1641620079,1641620079,-111.9959,40.7953,null,true,3.34,64.69,null,null,null,\"2632\",false,0,5],[\"ad4c96\",\"SKW5294 \",\"United States\",1641620081,1641620081,-111.9894,40.7237,1668.78,false,72.72,355.94,-4.88,null,1600.2,\"0637\",false,0,0],[\"abe7e3\",\"SWA3411 \",\"United States\",1641620070,1641620072,-111.9931,40.7908,null,true,7.72,174.38,null,null,null,\"7344\",false,0,1],[\"a2f721\",\"SKW4008 \",\"United States\",1641619837,1641619837,-111.9953,40.7912,null,true,6.43,120.94,null,null,null,\"1073\",false,0,0]]}";

            var response = JsonConvert.DeserializeObject<OverheadFlightService.OpenSkyStateResponse>(json, new OpenSkyStateJsonConverter());
            Assert.IsNotNull(response);
            Assert.That(response.States, Has.Count.EqualTo(4));
        }

    }
}