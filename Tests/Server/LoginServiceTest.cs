namespace PKiK.Tests.Server;

[TestClass]
public class LoginServiceTest {
    private readonly Config _config;
    private readonly LoginService _loginService;
    private readonly UserService _userService;

    public LoginServiceTest() {
        _config = Mocker.MockConfig();
        _loginService = new LoginService(new AuthenticationService(), _config);
        _userService = new UserService(_config);
    }

    [TestMethod]
    public void Register() {
        var result = _loginService.Register(DataGenerator.User());
        Assert.IsNotNull(result);
        Assert.IsTrue(ResponseValidator.IsHttpResponseValid(result));
    }

    [TestMethod]
    public void Login() {
        var result = _userService.GetUser("unittest");
        if (ResponseValidator.GetStatusCode(result) == 404) {
            result = _userService.AddUser(new UserDTO("unittest"));
            Assert.IsTrue(ResponseValidator.IsHttpResponseValid(result));
        }
        result = _loginService.Login("unittest", "unittest");
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
