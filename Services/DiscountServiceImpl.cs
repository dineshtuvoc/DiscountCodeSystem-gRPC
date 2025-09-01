using DiscountCode_App;
using DiscountCode_App.Models;
using DiscountCode_App.Helpers;
using Grpc.Core;

namespace DiscountCode_App.Services
{
    public class DiscountServiceImpl : DiscountService.DiscountServiceBase
    {
        public override Task<GenerateResponse> Generate(GenerateRequest request, ServerCallContext context)
        {
            var response = new GenerateResponse();

            // Validate count (ushort but max 2000 per spec)
            if (request.Count == 0 || request.Count > 2000)
            {
                response.Result = false;
                return Task.FromResult(response);
            }

            // Validate length (must be 7 or 8)
            if (request.Length < 7 || request.Length > 8)
            {
                response.Result = false;
                return Task.FromResult(response);
            }

            for (int i = 0; i < request.Count; i++)
            {
                string code = CodeGenerator.GenerateCode((int)request.Length);
                //Console.WriteLine($"[Generated] {code}");
            }

            response.Result = true;
            return Task.FromResult(response);
        }

        public override Task<UseCodeResponse> UseCode(UseCodeRequest request, ServerCallContext context)
        {
            var response = new UseCodeResponse();

            if (string.IsNullOrWhiteSpace(request.Code))
            {
                response.Result = 2; //not found
                return Task.FromResult(response);
            }

            // Check JSON for code
            bool success = CodeGenerator.UseCode(request.Code);

            if (!success)
            {
                // Either not found or already used
                var exists = CodeGenerator.CodeExists(request.Code);
                response.Result = exists ? 1 : 2;
            }
            else
            {
                response.Result = 0; // success
            }

            return Task.FromResult(response);
        }
    }
}
