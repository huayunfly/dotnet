﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace Workflow.Activities
{

    public sealed class ScheduleActivity : BaseActivity
    {
        // 定义一个字符串类型的活动输入参数
        public InArgument<string> Text { get; set; }

        // 如果活动返回值，则从 CodeActivity<TResult>
        // 并从 Execute 方法返回该值。
        protected override string Execute(CodeActivityContext context)
        {
            // 获取 Text 输入参数的运行时值
            string text = context.GetValue(this.Text);
            Console.WriteLine($"{ nameof(ScheduleActivity)}: {context.GetValue(this.Token)}");
            Console.WriteLine($"{ nameof(ScheduleActivity)}: {context.GetValue(this.Data)}");
            return text;
        }
    }
}
