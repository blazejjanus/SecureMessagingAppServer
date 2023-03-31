namespace PKiK.Tests.Server {
    [TestClass]
    public class LoginServiceTest {
        private LoginService loginService = new LoginService(new AuthenticationService());
        private UserService userService = new UserService();
        private Config config = Mocker.MockConfig();

        [TestMethod]
        public void Register() {
            var result = loginService.Register(DataGenerator.User());
            Assert.IsNotNull(result);
            Assert.IsTrue(ResponseValidator.IsHttpResponseValid(result));
        }

        [TestMethod]
        public void Login() {
            var result = userService.GetUser("unittest");
            if(ResponseValidator.GetStatusCode(result) == 404) {
                result = userService.AddUser(new UserDTO("unittest"));
                Assert.IsTrue(ResponseValidator.IsHttpResponseValid(result));
            }
            result = loginService.Login("unittest", "unittest");
            Assert.IsNotNull(result);
            Assert.IsTrue(ResponseValidator.IsHttpResponseValid(result));
        }

        [TestMethod]
        public void ValidateToken() {

        }

        [TestMethod]
        public void RevokeToken() {

        }
    }
}
