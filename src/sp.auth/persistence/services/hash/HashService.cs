using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sp.auth.app.interfaces;

namespace sp.auth.persistence.services.hash
{
    public class HashService : IHashService
    {
        private readonly ILogger<HashService> _logger;
        private HashAlgorithm _hashAlgorithm = null;

        private const string SaltKey = "sp.auth.salt";
        private readonly string _salt;

        private const string SaltAlgKey = "sp.auth.saltalg";
        private readonly string _saltAlg;


        public HashService(ILogger<HashService> logger, IConfiguration conf)
        {
            _logger = logger;
            
            _salt = conf[SaltKey];

            if (string.IsNullOrEmpty(_salt))
            {
                throw new KeyNotFoundException($"Salt key not found: {SaltKey}");
            }
            
            _saltAlg = conf[SaltAlgKey] ?? "";

            switch (_salt)
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
            var bytes = Encoding.ASCII.GetBytes($"{val}{_salt}");
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