namespace PKiK.Tests.Server;

[TestClass]
public class UserServiceTest {
    private readonly Config _config;
    private readonly UserService _userService;

    public UserServiceTest() {
        _config = Mocker.MockConfig();
        _userService = new UserService(_config);
    }

    [TestMethod]
    public void AddUser() {
        var result = _userService.AddUser(DataGenerator.User());
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
