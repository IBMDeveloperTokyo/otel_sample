# OpenTelemetry Getting Started for .Net

# 参考
以下を参考にしています。

https://opentelemetry.io/docs/instrumentation/net/getting-started/

## 環境
.Net 6 を使います。 <br/>
NuGetで公開されているOpenTelemetryパッケージ群が、本Readme作成時は.Net6までにしか対応していないためです。<br/>
(.Net 7、 .Net 8 への対応状況は、Nugetをご確認ください)

https://www.nuget.org/profiles/OpenTelemetry

### .Net SDK導入
Ubuntu の場合は apt で導入します。
```sh
sudo apt install dotnet-sdk-6.0
```

RHELやCentOS Streamをお使いの方は、バージョンにより導入方法が違うようです。<br/>
(例では.Net 7のインストール手順が示されていますが、.Net 6 を導入ください)

https://learn.microsoft.com/ja-jp/dotnet/core/install/linux-rhel

それ以外の環境は、Microsoftサイトよりご確認ください。

https://dotnet.microsoft.com/ja-jp/download/dotnet/6.0

## アプリ構築手順
ここでは、本リポジトリで公開しているプログラムは以下の手順で作成しました。

```sh
mkdir work_dir
cd work_dir
dotnet new web
dotnet add package OpenTelemetry.Extensions.Hosting
dotnet add package OpenTelemetry.Instrumentation.AspNetCore --prerelease
dotnet add package OpenTelemetry.Exporter.Console
dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol

(Program.cs 編集)
(Properties/launchSettins.json 編集)
```

### Service名変更
Program.csの中で、OpenTelemetryのサービス名を決めています。<br/>
これをそのまま実行すると、他の方とサービス名がかぶるので、かぶらない名前に変えましょう。

Program.csの定数 serviceName を定義している箇所で、文字列`otel-dojo-dotnet`をかぶらなさそうな名前にしてください。
```cs
const string serviceName = "otel-dojo-dotnet";
```

## ビルド、実行、確認
以下のコマンドでビルドを行いとアプリを実行します。
```cs
dotnet build
dotnet run
```

### 確認
動作確認をしましょう。他のターミナルなどで
```sh
curl http://localhost:8080/rolldice
```
を実行してみてください。<br/>
1〜6の数値、もしくは`System.Exception: 2の時は例外を投げます` という表示が出れば成功です。

## OpenTelemetry 動作確認　Console編

curlでアクセスすると、dotnet run を実行しているターミナルに以下が表示されましたか？<br/>
これがOpenTelemetryが表示しているTelemetry情報です。<br/>
みなさんの環境でも表示されているか確認ください。<br/>

Trace と Log として、以下が表示されます。
```
info: dotnet[0]
      Anonymous player is rolling the dice: 2
LogRecord.Timestamp:               2023-11-19T05:14:29.1500723Z
LogRecord.TraceId:                 ee763b43ce9b20a5ef9c467cc4a051d4
LogRecord.SpanId:                  4b6cad62c98dd3f0
LogRecord.TraceFlags:              Recorded
LogRecord.CategoryName:            dotnet
LogRecord.Severity:                Info
LogRecord.SeverityText:            Information
LogRecord.Body:                    Anonymous player is rolling the dice: {result}
LogRecord.Attributes (Key:Value):
    result: 2
    OriginalFormat (a.k.a Body): Anonymous player is rolling the dice: {result}

Resource associated with LogRecord:
service.name: otel-dojo-dotnet
service.instance.id: 765a8758-7c3e-4237-8d76-a22ff05611e9
telemetry.sdk.name: opentelemetry
telemetry.sdk.language: dotnet
telemetry.sdk.version: 1.6.0

fail: dotnet[0]
      2なのでErrorです
LogRecord.Timestamp:               2023-11-19T05:14:29.1506143Z
LogRecord.TraceId:                 ee763b43ce9b20a5ef9c467cc4a051d4
LogRecord.SpanId:                  4b6cad62c98dd3f0
LogRecord.TraceFlags:              Recorded
LogRecord.CategoryName:            dotnet
LogRecord.Severity:                Error
LogRecord.SeverityText:            Error
LogRecord.Body:                    2なのでErrorです
LogRecord.Attributes (Key:Value):
    OriginalFormat (a.k.a Body): 2なのでErrorです

Resource associated with LogRecord:
service.name: otel-dojo-dotnet
service.instance.id: 765a8758-7c3e-4237-8d76-a22ff05611e9
telemetry.sdk.name: opentelemetry
telemetry.sdk.language: dotnet
telemetry.sdk.version: 1.6.0

fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.Exception: 2の時は例外を投げます
         at Program.<>c__DisplayClass0_0.<<Main>$>g__HandleRollDice|4(String player) in /home/tetsu/work/otel_sample/dotnet/Program.cs:line 56
         at lambda_method1(Closure , Object , HttpContext )
         at Microsoft.AspNetCore.Http.RequestDelegateFactory.<>c__DisplayClass36_0.<Create>b__0(HttpContext httpContext)
         at Microsoft.AspNetCore.Routing.EndpointMiddleware.Invoke(HttpContext httpContext)
      --- End of stack trace from previous location ---
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)
LogRecord.Timestamp:               2023-11-19T05:14:29.1513490Z
LogRecord.TraceId:                 ee763b43ce9b20a5ef9c467cc4a051d4
LogRecord.SpanId:                  4b6cad62c98dd3f0
LogRecord.TraceFlags:              Recorded
LogRecord.CategoryName:            Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware
LogRecord.Severity:                Error
LogRecord.SeverityText:            Error
LogRecord.Body:                    An unhandled exception has occurred while executing the request.
LogRecord.Attributes (Key:Value):
    OriginalFormat (a.k.a Body): An unhandled exception has occurred while executing the request.
LogRecord.EventId:                 1
LogRecord.EventName:               UnhandledException
LogRecord.Exception:               System.Exception: 2の時は例外を投げます
   at Program.<>c__DisplayClass0_0.<<Main>$>g__HandleRollDice|4(String player) in /home/tetsu/work/otel_sample/dotnet/Program.cs:line 56
   at lambda_method1(Closure , Object , HttpContext )
   at Microsoft.AspNetCore.Http.RequestDelegateFactory.<>c__DisplayClass36_0.<Create>b__0(HttpContext httpContext)
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.Invoke(HttpContext httpContext)
--- End of stack trace from previous location ---
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)

Resource associated with LogRecord:
service.name: otel-dojo-dotnet
service.instance.id: 765a8758-7c3e-4237-8d76-a22ff05611e9
telemetry.sdk.name: opentelemetry
telemetry.sdk.language: dotnet
telemetry.sdk.version: 1.6.0

Activity.TraceId:            ee763b43ce9b20a5ef9c467cc4a051d4
Activity.SpanId:             4b6cad62c98dd3f0
Activity.TraceFlags:         Recorded
Activity.ActivitySourceName: OpenTelemetry.Instrumentation.AspNetCore
Activity.DisplayName:        GET /rolldice/{player?}
Activity.Kind:               Server
Activity.StartTime:          2023-11-19T05:14:28.9497009Z
Activity.Duration:           00:00:00.2056614
Activity.Tags:
    net.host.name: localhost
    net.host.port: 8080
    http.method: GET
    http.scheme: http
    http.target: /rolldice
    http.url: http://localhost:8080/rolldice
    http.flavor: 1.1
    http.user_agent: curl/7.81.0
    http.route: /rolldice/{player?}
    http.status_code: 500
StatusCode: Error
Resource associated with Activity:
    service.name: otel-dojo-dotnet
    service.instance.id: f36c8c09-92e4-44d9-b96b-ff850c0e7060
    telemetry.sdk.name: opentelemetry
    telemetry.sdk.language: dotnet
    telemetry.sdk.version: 1.6.0
```

Metric として、以下が表示されます。
```
Export http.server.duration, Measures the duration of inbound HTTP requests., Unit: ms, Meter: OpenTelemetry.Instrumentation.AspNetCore/1.0.0.0
(2023-11-19T05:13:01.3422973Z, 2023-11-19T05:14:31.2971521Z] http.flavor: 1.1 http.method: GET http.route: /rolldice/{player?} http.scheme: http http.status_code: 200 net.host.name: localhost net.host.port: 8080 Histogram
Value: Sum: 946.5857000000001 Count: 3 Min: 102.1588 Max: 500.8253
(-Infinity,0]:0
(0,5]:0
(5,10]:0
(10,25]:0
(25,50]:0
(50,75]:0
(75,100]:0
(100,250]:1
(250,500]:1
(500,750]:1
(750,1000]:0
(1000,2500]:0
(2500,5000]:0
(5000,7500]:0
(7500,10000]:0
(10000,+Infinity]:0
```

## OpenTelemetry 動作確認　Instana編
Consoleの表示が確認できたら、続けてInstanaにテレメトリデータが表示されることを確認しましょう。<br/>
まずは、定期的に、アプリへのリクエストを送りましょう。例えば以下です。
```sh
while true; do curl http://localhost:8080/rolldice; sleep 1; done
```

もしくは、このREADME.mdと同ディレクトリにある　　curl_exec.sh　を実行していただいてもOKです。

実行を開始したら、1分くらいしたところで　Instana の動作を確認しましょう。<br/>
メニューから「アプリケーション」を選択し、「サービス」タグをクリックします。<br/>
みなさんが Program.cs の定数``にセットしたサービス名が表示されていますか？　<br/>
もし表示されていない場合は時間範囲を「過去5分間」で「稼働中」ボタンをクリックしリアルタイム状態にしてみて下さい。

ここでは Instana が OpenTelemetry の情報をどのように表現しているか、説明は控えます。<br/>
ぜひ実際のInstanaの画面を操作し体感してください！
