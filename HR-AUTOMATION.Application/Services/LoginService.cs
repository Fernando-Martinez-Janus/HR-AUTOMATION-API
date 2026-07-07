using FluentValidation.Results;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using Shared.Kernel.JWT;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;


namespace HR_AUTOMATION.Application
{
    public class LoginService
    {
        private readonly JWTService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public LoginService(JWTService jwtService, IUserRepository userRepository, IPasswordService passwordService)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<string> ExecuteAsync(LoginInputModel loginInputModel)
        {
            ValidationResult validationResult = new LoginValidator().Validate(loginInputModel);
            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                throw new ResponseExceptionFactory(currentException);
            }


            var user = await _userRepository.GetByEmailAsync(loginInputModel.Email);
            if (user == null)
            {
                throw new ResponseExceptionFactory(Exceptions.InvalidCredentials);
            }


            bool isPasswordValid = _passwordService.VerifyPassword(loginInputModel.Password, user.Password);
            if (!isPasswordValid)
            {
                throw new ResponseExceptionFactory(Exceptions.InvalidCredentials);
            }




            return await _jwtService.GenerateJWT(user.Username, user.Email);
        }
    }
}