# メモ
## WPF の帳票機能の調査
- 横向きに印刷するには、コンテントを横向きでレイアウトしつつ90度回転させることで、縦向きの紙に印刷する。
    - RenderTransform に RotateTransform を指定する。
    - プリンターに PageOrientation.Landscape を指定する方法はうまくいかない (筆者調べ)。
- DocumentViewer を使うことで、ズームしたり2ページごとに並べたり印刷ダイアログを表示したりできる。紙のサイズや縦横を指定できないので、いまいち嬉しくない。
- DocumentPaginator を使うことでページネーションをカスタマイズできる。いまいち嬉しくない。
- FlowDocument と FlowDocumentPageViewer が使えるかもしれない。筆者の用途に合わなかったので、詳しくは調べてない。

## 問題
- ページネーションやレイアウトの計算にかなり時間がかかる。
    - ページ数が多い場合はレイアウトの簡略化などの工夫がいる。
    - プレビューは手動で更新したほうがいい。
- ページネーション時、各ページは毎回生成したほうがいい。
    - ページごとにレイアウトが異なることがあるため。
    - スクロールする必要はなくなった。
- 「ページ数が 1 のときにページ番号表記を非表示にする」ようにすると、正しくページネーションできない。ページネーションする段階ではページ数が 1 なので、「ページ番号表記がない」ときのレイアウトでページネーションしてしまう。
- ページネーションの間、UIが応答しない。
    - 別のUIスレッドを使う？
    - 参考
        - <https://stackoverflow.com/questions/5716804/can-does-wpf-have-multiple-gui-threads>
        - <https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/threading-model>
- eap to tap 変換に single assignment disposable が便利！

## ライブラリーの実装
- PrintableDataGrid は通常のウィンドウに貼り付けるのには向いていない。
- その他: progress/cancellation があったほうがいいかもしれない。
