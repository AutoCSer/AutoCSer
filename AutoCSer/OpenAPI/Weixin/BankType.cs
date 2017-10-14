using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 银行类型
    /// </summary>
    public enum BankType : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 工商银行（借记卡）
        /// </summary>
        ICBC_DEBIT,
        /// <summary>
        /// 工商银行（信用卡）
        /// </summary>
        ICBC_CREDIT,
        /// <summary>
        /// 农业银行（借记卡）
        /// </summary>
        ABC_DEBIT,
        /// <summary>
        /// 农业银行 （信用卡）
        /// </summary>
        ABC_CREDIT,
        /// <summary>
        /// 邮政储蓄（借记卡）
        /// </summary>
        PSBC_DEBIT,
        /// <summary>
        /// 邮政储蓄 （信用卡）
        /// </summary>
        PSBC_CREDIT,
        /// <summary>
        /// 建设银行（借记卡）
        /// </summary>
        CCB_DEBIT,
        /// <summary>
        /// 建设银行 （信用卡）
        /// </summary>
        CCB_CREDIT,
        /// <summary>
        /// 招商银行（借记卡）
        /// </summary>
        CMB_DEBIT,
        /// <summary>
        /// 招商银行（信用卡）
        /// </summary>
        CMB_CREDIT,
        /// <summary>
        /// 交通银行（借记卡）
        /// </summary>
        COMM_DEBIT,
        /// <summary>
        /// 中国银行（信用卡）
        /// </summary>
        BOC_CREDIT,
        /// <summary>
        /// 浦发银行（借记卡）
        /// </summary>
        SPDB_DEBIT,
        /// <summary>
        /// 浦发银行 （信用卡）
        /// </summary>
        SPDB_CREDIT,
        /// <summary>
        /// 广发银行（借记卡）
        /// </summary>
        GDB_DEBIT,
        /// <summary>
        /// 广发银行（信用卡）
        /// </summary>
        GDB_CREDIT,
        /// <summary>
        /// 民生银行（借记卡）
        /// </summary>
        CMBC_DEBIT,
        /// <summary>
        /// 民生银行（信用卡）
        /// </summary>
        CMBC_CREDIT,
        /// <summary>
        /// 平安银行（借记卡）
        /// </summary>
        PAB_DEBIT,
        /// <summary>
        /// 平安银行（信用卡）
        /// </summary>
        PAB_CREDIT,
        /// <summary>
        /// 光大银行（借记卡）
        /// </summary>
        CEB_DEBIT,
        /// <summary>
        /// 光大银行（信用卡）
        /// </summary>
        CEB_CREDIT,
        /// <summary>
        /// 兴业银行 （借记卡）
        /// </summary>
        CIB_DEBIT,
        /// <summary>
        /// 兴业银行（信用卡）
        /// </summary>
        CIB_CREDIT,
        /// <summary>
        /// 中信银行（借记卡）
        /// </summary>
        CITIC_DEBIT,
        /// <summary>
        /// 中信银行（信用卡）
        /// </summary>
        CITIC_CREDIT,
        /// <summary>
        /// 深发银行（信用卡）
        /// </summary>
        SDB_CREDIT,
        /// <summary>
        /// 上海银行（借记卡）
        /// </summary>
        BOSH_DEBIT,
        /// <summary>
        /// 上海银行 （信用卡）
        /// </summary>
        BOSH_CREDIT,
        /// <summary>
        /// 华润银行（借记卡）
        /// </summary>
        CRB_DEBIT,
        /// <summary>
        /// 杭州银行（借记卡）
        /// </summary>
        HZB_DEBIT,
        /// <summary>
        /// 杭州银行（信用卡）
        /// </summary>
        HZB_CREDIT,
        /// <summary>
        /// 包商银行（借记卡）
        /// </summary>
        BSB_DEBIT,
        /// <summary>
        /// 包商银行 （信用卡）
        /// </summary>
        BSB_CREDIT,
        /// <summary>
        /// 重庆银行（借记卡）
        /// </summary>
        CQB_DEBIT,
        /// <summary>
        /// 顺德农商行 （借记卡）
        /// </summary>
        SDEB_DEBIT,
        /// <summary>
        /// 深圳农商银行（借记卡）
        /// </summary>
        SZRCB_DEBIT,
        /// <summary>
        /// 哈尔滨银行（借记卡）
        /// </summary>
        HRBB_DEBIT,
        /// <summary>
        /// 成都银行（借记卡）
        /// </summary>
        BOCD_DEBIT,
        /// <summary>
        /// 南粤银行 （借记卡）
        /// </summary>
        GDNYB_DEBIT,
        /// <summary>
        /// 南粤银行 （信用卡）
        /// </summary>
        GDNYB_CREDIT,
        /// <summary>
        /// 广州银行（信用卡）
        /// </summary>
        GZCB_CREDIT,
        /// <summary>
        /// 江苏银行（借记卡）
        /// </summary>
        JSB_DEBIT,
        /// <summary>
        /// 江苏银行（信用卡）
        /// </summary>
        JSB_CREDIT,
        /// <summary>
        /// 宁波银行（借记卡）
        /// </summary>
        NBCB_DEBIT,
        /// <summary>
        /// 宁波银行（信用卡）
        /// </summary>
        NBCB_CREDIT,
        /// <summary>
        /// 南京银行（借记卡）
        /// </summary>
        NJCB_DEBIT,
        /// <summary>
        /// 青岛银行（借记卡）
        /// </summary>
        QDCCB_DEBIT,
        /// <summary>
        /// 浙江泰隆银行（借记卡）
        /// </summary>
        ZJTLCB_DEBIT,
        /// <summary>
        /// 西安银行（借记卡）
        /// </summary>
        XAB_DEBIT,
        /// <summary>
        /// 常熟农商银行 （借记卡）
        /// </summary>
        CSRCB_DEBIT,
        /// <summary>
        /// 齐鲁银行（借记卡）
        /// </summary>
        QLB_DEBIT,
        /// <summary>
        /// 龙江银行（借记卡）
        /// </summary>
        LJB_DEBIT,
        /// <summary>
        /// 华夏银行（借记卡）
        /// </summary>
        HXB_DEBIT,
        /// <summary>
        /// 测试银行借记卡快捷支付 （借记卡）
        /// </summary>
        CS_DEBIT,
        /// <summary>
        /// AE （信用卡）
        /// </summary>
        AE_CREDIT,
        /// <summary>
        /// JCB （信用卡）
        /// </summary>
        JCB_CREDIT,
        /// <summary>
        /// MASTERCARD （信用卡）
        /// </summary>
        MASTERCARD_CREDIT,
        /// <summary>
        /// VISA （信用卡）
        /// </summary>
        VISA_CREDIT
    }
}
