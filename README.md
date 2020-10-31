# AvabilityChecker
查果12(Pro)库存的小玩意

## 咋跑起来
打开AvabilityChecker.sln，生成，运行

或者懒得生成的话，去Releases里拉一个

默认查询国服 12 Pro库存，按F1切换 12 库存 <i>（但是这玩意会缺货吗）</i>

## 跑起来之后呢
- 空格：显示每家店所有型号的库存
>以下为举例
>>成都太古里
>>
>>MGLL3CH/A cont:False unlock:False
>>
>>Last Update:2020/10/31 下午8:29:00
>
>表明截止 20点29分 成都太古里的这个型号完全没货，接着等着吧。

- 回车：显示每家店的有货库存
- F1:切换iPhone 12/iPhone 12 Pro模式
- Esc:退出

## 有库存的话呢
程序会每10秒钟刷新一次库存（虽然水果自己是30秒更新一次）

一旦出现库存程序会立即提示，并打印跳转链接
> 以下为举例
>
> \*\*\*Stocks:成都太古里 MGLL3CH/A \*\*\*
>
> http s://前略/A?store=R400&partNumber=MGLL3CH/A(后略)

点击链接即可直接跳转浏览器进入到预订画面
