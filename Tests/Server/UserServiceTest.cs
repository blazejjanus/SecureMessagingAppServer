namespace PKiK.Tests.Server {
    [TestClass]
    public class UserServiceTest {
        private UserService service = new UserService();
        private Config config = Mocker.MockConfig();

        [TestMethod]
        public void AddUser() {
            var result = service.AddUser(DataGenerator.User());
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
