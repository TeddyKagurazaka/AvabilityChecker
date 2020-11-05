# AvabilityChecker
查果12(Pro/mini)库存的小玩意

## 咋跑起来
打开AvabilityChecker.sln，生成，运行

或者懒得生成的话，去Releases里拉一个

默认查询国服 12 Pro库存，按F1切换 12 Pro Max/12/12 mini 库存

<i>注:截止更新时还不清楚哪个渠道代表12PM/12mini</i>

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
- F1:切换模式,iPhone 12 Pro -> Pro Max? -> Pro Max(年年换新)? -> 12 -> 12 mini? -> 12 mini(年年换新)?
- F2:打开/关闭提示音,发现任意库存的时候会哔一声(提醒用户该冲商城了)
- F3:轮盘转模式,每次扫库存将扫全部渠道的库存(除了iPhone 12,这个不缺货)
- Esc:退出

## 有库存的话呢
程序会每10秒钟刷新一次库存（虽然水果自己是30秒更新一次）

一旦出现库存程序会立即提示
> 以下为举例
>
> \*\*\*Stocks:成都太古里 MGLL3CH/A \*\*\*

## Stocks.csv
程序会记录所有抓到的库存信息并保存到Stocks.csv内,格式如下

> 系统当前时间,库存刷新时间,店名,店ID,型号,对应机型,渠道(A/B/C/F/G/H)
>
> 如: 12:34:56,12:34:30,成都太古里,R580,MGLF3CH/A,白256,A

根据抢货经验分析,每家店的上架时间 <b>并不一定为早上6点</b>, 因此通过记录的方式可以统计上新规律(也许)

## 更新日志
[ChangeLog](../master/ChangeLog.md)