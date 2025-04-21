using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumUtil
{
    /// <summary>
    /// ��ҹ��� �����ϰ� enum �Ľ��ϴ� �Լ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryParseIgnoreCase<T>(string input, out T result) where T : struct, Enum
    {
        return Enum.TryParse(input, true, out result);
    }
}
