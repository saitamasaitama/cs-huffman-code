using System;
using System.Linq;
using System.IO;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

namespace cs_huffman_code
{
  class Program
  {
    static void Main(string[] args)
    {
      HuffmanData<int> result=(
      HuffmanCode.Encode<int>(new int[]{
        111,222,333,
        222,333,
        333,
          }));
      Console.Write($"LENGTH={result.ToBytes().Length}Byte\n");
     
      List<int> decode= HuffmanCode.Decode<int>(result);
      foreach (bool b in result.body)
      {
        Console.Write($"{(b?1:0)},");
      }
      Console.WriteLine();

      foreach (KeyValuePair<int,bool[]> kv in result.header)
      {
        Console.WriteLine($"{kv.Key}:");
        foreach(bool b in kv.Value)
        {
          Console.Write($"{(b ? 1 : 0)},");
        }
        Console.WriteLine();
      }

      Console.WriteLine();

      
      foreach (int byt in decode)
      {
        Console.Write($"{byt},");
      }
      
    }
  }
}

public class HuffmanCode {

  public static List<T> Decode<T>(HuffmanData<T> data){
    var result = new List<T>();

    int count = 0;
    //dataから読み込み
    foreach(bool b in data.body)
    {
      if (b)
      {
        //逆indexを取得するべし
        result.Add(data.indexTable[count]);
        count = 0;
        continue;
      }
      count++;
    }

    return result;
  }

  public static HuffmanData<T> Encode<T>(T[] source){
    var count=new Dictionary<T,int>();

    //カウント処理
    foreach(T data in source ){
      if(!count.ContainsKey(data)){
        count[data] = 0;
      }
      count[data]++;
    }


    var list= count.OrderByDescending(v=>v.Value)
      .Select(v=>v.Key).ToList();
    var table=new Dictionary<T,bool[]>();
    
    //テーブルを生成
    for(int i=0;i<list.Count();i++){
      
      table.Add(list[i], createHuffman(i));
    }
    var body=new HuffmanCodeBody();

    //符号化処理
    foreach(T data in source){
      body.AddRange(table[data]);
    }

    return new HuffmanData<T>(){
      header=table,
      indexTable=list,
      body=body
    };
  }

  private static bool[] createHuffman(int num)
  {
    var result = new List<bool>();
    
    for(int i = 0; i < num; i++)
    {
      result.Add(false);
    }
    result.Add(true);
    return result.ToArray();
  }
}

public class HuffmanData<T>{
  public List<T> indexTable;
  public Dictionary<T,bool[]> header;//ヘッダ
  public HuffmanCodeBody body;//実質的なバイナリ配列

  public Byte[] ToBytes() {
    var bits= new BitArray(body.ToArray());
    var memory = new MemoryStream();

    using (BinaryWriter writer=new BinaryWriter(memory))
    {
      for(int byteCount=0;byteCount < (bits.Count/8) + 1; byteCount++)
      {
        Byte b = 0;
        //1を書き込み
        for(int i = 0; i < 8; i++)
        {
          int index = (byteCount * 8 + i);
          if (bits.Count <= index) break;
          //オーバーするなら終了
          if (!bits[index]) continue;
          b =(byte)(b|(1<<i));
        }
        writer.Write(b);
      }
     
      return memory.ToArray();
    }
  }
}


public class HuffmanCodeBody:List<bool>{
  public override string ToString(){
    string s="";
    foreach(bool b in this ){
      s+=(b?"T":"F");
    }
    return s;
  }

}

