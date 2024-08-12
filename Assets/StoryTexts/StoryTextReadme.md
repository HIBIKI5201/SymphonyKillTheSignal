# 概要
TextListのコマンド一覧

#基礎
Text以外のKindでは改行は引数に相当する

##Text
文字列をテキストボックスに表示する
改行も対応
三行までとする

##Move
対象のGameObjectのAnimatorのStateを呼び出す
第二引数にアニメーション中何秒間テキスト更新を遅らせるか数値を入れる
第二引数が無い場合1秒間になる

##Sound
第一引数に種類を入れる
第一引数が0だとSoundEffect、1だとBGMのリスト
第二引数にはリストのIndexを入れる