using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AhpilyServer
{
   public  class EncodeTool
    {
        public static byte[] EncodePacket(byte[] data)
        {
            using (MemoryStream ms=new MemoryStream())
            {
                using (BinaryWriter bw=new BinaryWriter(ms))
                {
                    bw.Write(data.Length);
                    bw.Write(data);

                    byte[] byteArrary = new byte[(int)ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(),0,byteArrary,0,(int)ms.Length);

                    return byteArrary;
                }
            }
        }

        public static byte[] DecodePacket(ref List<byte> dataCache)
        {
            if (dataCache.Count < 4)
                throw new Exception("数据缓存长度不足4 不能构成一个完整的消息");

            using (MemoryStream ms=new MemoryStream(dataCache.ToArray()))
            {
                using (BinaryReader br=new BinaryReader(ms))
                {
                    int length = br.ReadInt32();
                    int dataRemainLength = (int)(ms.Length - ms.Position);
                    if (length > ms.Length - ms.Position)
                        throw new Exception("数据长度不够约定的长度 不能构成一个完整的消息");

                    byte[] data = br.ReadBytes(length);

                    //更新数据缓存
                    dataCache.Clear();
                    dataCache.AddRange(br.ReadBytes(dataRemainLength));

                    return data;
                }
            }
        }
    }
}
