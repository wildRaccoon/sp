using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sp.auth.app.infra.config;
using sp.auth.app.interfaces;

namespace sp.auth.persistence.services.hash
{
    public class HashService : IHashService
    {
        private readonly ILogger<HashService> _logger;
        private readonly HashConfig _hashConfig;
        private HashAlgorithm _hashAlgorithm = null;

        public HashService(ILogger<HashService> logger, HashConfig hashConfig)
        {
            _logger = logger;
            _hashConfig = hashConfig ?? throw new ArgumentNullException(nameof(hashConfig));;

            if (string.IsNullOrEmpty(_hashConfig.salt))
            {
                throw new KeyNotFoundException($"Salt not set in secret: sp.auth.authenticate.Salt");
            }
            
            var _saltAlg = _hashConfig.algorithm?.ToLower() ?? "";

            switch (_saltAlg)
            {
                case "sha256":
                    _hashAlgorithm = SHA256.Create();
                    break;
                case "sha384":
                    _hashAlgorithm = SHA384.Create();
                    break;
                case "sha512":
                    _hashAlgorithm = SHA512.Create();
                    break;
                default:
                    _hashAlgorithm = MD5.Create();
                    break;
            }
        }
        
        public string Encode(string val)
        {
            var bytes = Encoding.ASCII.GetBytes($"{val}{_hashConfig.salt}");
            var hashVal = _hashAlgorithm.ComputeHash(bytes);
            
            var builder = new StringBuilder();  
            foreach (var t in hashVal)
            {
                builder.Append(t.ToString("x2"));
            }  
            return builder.ToString(); 
        }
    }
}