
using System;
using System.Management;


Console.WriteLine("🔍 USB 컨트롤러 목록 조회 중...");

using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBController"))
{
    foreach (var usb in searcher.Get())
    {
        string name = usb["Name"].ToString();
        Console.WriteLine($"🔌 USB 컨트롤러: {name}");

        if (name.Contains("3.0") || name.Contains("eXtensible"))
        {
            Console.WriteLine("✅ 이 컴퓨터에는 USB 3.0 포트가 있습니다.");
        }
        else if (name.Contains("2.0") || name.Contains("OpenHCD"))
        {
            Console.WriteLine("⚠️ USB 2.0 포트가 있습니다.");
        }
    }
}