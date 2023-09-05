using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IAuthenticationService
    {
        //Register User
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);
        //Validate User
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuthDto);
        //create token
        Task<TokenDto> CreateToken(bool populateExp);

        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
