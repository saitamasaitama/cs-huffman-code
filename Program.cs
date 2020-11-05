using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace cs_huffman_code
{
  class Program
  {
    static void Main(string[] args)
    {
      var b=(
      HuffmanCode.Encode<int>(new int[]{
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          99999,222222,888888,22222,22222,
          1,2,4,1,5,1,2,
          4,1,3,4,2,5,2,1,3,1,
          44,2,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3
          }).body);
      Console.Write($"LENGTH={b.Count/8}Byte");
      Console.Write($"ORIGIN={b}");
    }
  }
}

public class HuffmanCode {

  public static List<T> Decode<T>(HuffmanData<T> data){

    return null;
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
      //ビット配列を追加
      var v=new List<bool>();
      for(int j=1;j<i;j++){
        v.Add(false);
      }
      v.Add(true);
      table.Add(list[i],v.ToArray());
    }

    var header=table;
    var body=new HuffmanEncodeBody();

    //符号化処理
    foreach(T data in source){
      body.AddRange(table[data]);
    }


    return new HuffmanData<T>(){
      header=header,
      body=body
    };
  }
}

public class HuffmanData<T>{
  public Dictionary<T,bool[]> header;//ヘッダ
  public HuffmanEncodeBody body;//実質的なバイナリ配列
}


public class HuffmanEncodeBody:List<bool>{
  public override string ToString(){
    string s="";
    foreach(bool b in this ){
      s+=(b?"T":"F");
    }
    return s;
  }
  public byte[] ToBytes(){

    //ここに処理を書く
    return null;
  }
}

