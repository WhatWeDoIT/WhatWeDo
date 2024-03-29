﻿using System.Security.Cryptography;
using System.Text;

namespace WhatWeDo.Recursos
{
    public class Utilidades
    {
        public static string EncriptarPassword(string sPassword)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding encoding = Encoding.UTF8;

                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(sPassword));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
