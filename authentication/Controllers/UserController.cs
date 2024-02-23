﻿using authentication.DTOs;
using authentication.Models;
using authentication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<List<User?>>> Register(UserInputDTO userDTO)
        {
            var response = await _userService.CreateUser(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginInputDTO loginDTO)
        {
            var response = await _userService.Login(loginDTO);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<User?>> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User?>> UpdateUser(int id, UserUpdateDTO userUpdateDTO)
        {
            var response = await _userService.UpdateUser(id, userUpdateDTO);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User?>> GetSingleUserById(int id)
        {
            var response = await _userService.GetSingleUserById(id);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User?>> DeleteUserById(int id)
        {
            var response = await _userService.DeleteUserById(id);
            return Ok(response);
        }
    }
}
