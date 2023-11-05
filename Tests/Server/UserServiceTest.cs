namespace PKiK.Tests.Server {
    [TestClass]
    public class UserServiceTest {
        private Config config;
        private UserService userService;

        public UserServiceTest() {
            config = Mocker.MockConfig();
            userService = new UserService(config);
        }

        [TestMethod]
        public void AddUser() {
            var result = userService.AddUser(DataGenerator.User());
            Assert.IsTrue(ResponseValidator.IsHttpResponseValid(result));
        }

        [TestMethod]
        public void RemoveUser() {

        }

        [TestMethod]
        public void ModifyUser() {

        }

        [TestMethod]
        public void GetUser() {

        }
    }
}
