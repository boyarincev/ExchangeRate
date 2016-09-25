;'use strict';
var app = (function () {
    return {
        init: init,
        updateChart: updateChart
    };

    var _updateUrl;
    var _monthsDropDown;
    var _currenciesDropDown;
    var _highchartContainer;

    function init(updateUrl, monthsDropDown, currenciesDropDown, highchartContainer) {
        _updateUrl = updateUrl;
        _monthsDropDown = monthsDropDown;
        _currenciesDropDown = currenciesDropDown;
        _highchartContainer = highchartContainer;

        updateChart();

        _monthsDropDown.change(function () {
            updateChart();
        });

        _currenciesDropDown.change(function () {
            updateChart();
        });
    }

    function updateChart() {
        var choosenMonth = _monthsDropDown.val();
        var choosenCurrency = _currenciesDropDown.val();
        var choosenCurrencyName = _currenciesDropDown.children("option:selected").text();

        //TODO добавить индикацию ожидания ответа
        var jqXHR = $.post(_updateUrl, {
            month: choosenMonth,
            currencyCode: choosenCurrency,
            currencyName: choosenCurrencyName
        }, updateChartCallback).fail(errorCallback);
    }

    function updateChartCallback(data) {
        var categories = data.OrderedByDateExchangeRates ? data.OrderedByDateExchangeRates.map(function (er) { return er.DayNumber }) : [];
        var values = data.OrderedByDateExchangeRates ? data.OrderedByDateExchangeRates.map(function (er) { return er.Value }) : [];
        var currnecyName = data.CurrencyName ? data.CurrencyName : "";

        _highchartContainer.highcharts({
            title: {
                text: 'График изменения валюты',
                x: -20 //center
            },
            subtitle: {
                text: 'Источник: www.cbr.ru',
                x: -20
            },
            xAxis: {
                categories: categories,
                title: {
                    text: 'День'
                }
            },
            yAxis: {
                title: {
                    text: 'Стоимость валюты в рублях'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                valueSuffix: ''
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: [{
                name: currnecyName,
                data: values
            }]
        });
    }

    function errorCallback() {
        //TODO заменить чем-нибудь более красивым
        alert("Произошла ошибка, обратитесь к администратору");
    }

})();