namespace Elliptical.Mvc.Identity
{
    public interface IApiLoginViewModel
    {
        string ReturnUrl { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool RememberMe { get; set; }
        
    }
}