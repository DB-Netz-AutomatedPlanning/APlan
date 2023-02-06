using aplan.core;
using APLan.Model.aplan;

namespace APlanTest
{
    public class Model_aplan_HelperClassTest
    {
        [Fact]
        public void roundUp_Test()
        {
            double input = 67.788923;
            int precision = 2;

            double actual = Helper.roundUp(input, precision);

            Assert.Equal(67.79, actual);
        }


        [Fact]
        public void hmToKm_Test()
        {
            string hectometerPlusMeter = "74,7 + 52";
            double expected = 74.75200000000001;

            double? actual = Helper.hmToKm(hectometerPlusMeter);
            Assert.Equal(expected, actual);
        }
    }
}