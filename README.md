# JsonQL

Responsive Json Query Language，客户端发送伪Json查询，服务端返回查询结果，实现所查即所得。  
基于 DynamicLinq 支持常用的Linq查询。  
基于 DynamicEvaluation 支持表达式计算。

### 语法
##### 变量的定义：\$变量名
变量必须先定义后使用，所有在定义前的引用都是错误的。  
变量具有作用域，只在当前定义它的 “\{}” 及嵌套的子 “\{}” 内有效，同时子作用域会覆盖父作用域定义的同名变量。  
> 例：$abc。
  
##### 资源的引用：资源名[]
资源名必须与服务端公开的资源列表中的名称一致并区分大小写。
> 例：users[]。
##### 方法的调用：.方法名(参数)
> 例：.where(age>=18).count()。
##### 字段的访问：.字段名
> 例：.user.name。
##### 资源的枚举：=>\$
使用资源访问符 “=>” 对资源进行枚举，“=>” 左侧定义资源的引用，右侧定义资源的输出（类似javascript对象）。  
如果右侧定义以 “[]” 结尾，则表示遍历资源输出一个数组，否则表示只输出索引为0的元素。  
“\$” 表示对资源中元素的引用。  
定义输出时，输出字段与元素字段名称一致时，可以省略 “\$.”。  
> 例：users[] => { uuid:$.id, name, age }
##### 使用表达式：**{{表达式}}**
使用表达式可以执行一些运算操作，包括四则运算、逻辑运算、位运算等。
表达式必须使用“{{}}”包括，其运算结果为一个值。
表达式中可以使用变量，可以访问属性，但不可使用资源，不可调用方法。
> 例：{{ 1 + 2 - 3 * 4 / 5 % 6 }}、{{ $abc > 0 }}、{{ true ? 1 : 0 }}

### Demo
```js
{
    //定义资源变量
    $users: users[],
    $orders: orders[].where(user.id==1),

    //获取用户信息
    user: $users.where(id==1) =>
    {
        uuid: $.id,                             //Id
        username,                               //用户名
        avatar,                                 //头像
        account:
        {
            coins: $.account.coins,
            points: $.account.points
        }
    },
    //订单不同状态下的数量
    orderCount:
    { 
        created: $orders.count(status==1),
        payed: $orders.count(status==2),
        delivered: $orders.count(status==3),
        completed: $orders.count(status==4)
    },
    //获取该用户的订单列表，前10条数据
    orders: $orders.orderBy(createTime).take(10) =>
    {
        uuid: $.id,                             //Id
        serial,                                 //订单号
        status,                                 //订单状态
        statusChanges: $.statusChanges.orderby(changeTime) =>
        {
            changeTime,
            status,
            remark
        }[]
    }[]
}
```
