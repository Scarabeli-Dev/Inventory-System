
var dom = document.getElementById('chart-container');
var myChart = echarts.init(dom, null, {
    renderer: 'canvas',
    useDirtyRect: false
});
var app = {};

var option;
option = {
    series: [
        {
            type: 'gauge',
            startAngle: 180,
            endAngle: 0,
            center: ['50%', '75%'],
            radius: '90%',
            min: 0,
            max: 1,
            splitNumber: 8,
            axisLine: {
                lineStyle: {
                    width: 35,
                    color: [
                        [0.85, 'rgba(250, 32, 12, 1)'],
                        [0.90, '#FDDD60'],
                        [0.95, '#58D9F9'],
                        [1, '#7CFFB2']
                    ]
                }
            },
            pointer: {
                icon: 'path://M12.8,0.7l12,40.1H0.7L12.8,0.7z',
                length: '18%',
                width: 20,
                offsetCenter: [0, '-65%'],
                itemStyle: {
                    color: 'auto'
                }
            },
            axisTick: {
                length: 12,
                lineStyle: {
                    color: 'auto',
                    width: 2
                }
            },
            splitLine: {
                length: 20,
                lineStyle: {
                    color: 'auto',
                    width: 5
                }
            },
            axisLabel: {
                color: '#464646',
                fontSize: 20,
                distance: -60,
                rotate: 'horizontal',
                formatter: function (value) {
                    if (value === 0.875) {
                        return '';
                    } else if (value === 0.625) {
                        return '';
                    } else if (value === 0.375) {
                        return '';
                    } else if (value === 0.125) {
                        return '';
                    }
                    return '';
                }
            },
            title: {
                offsetCenter: [0, '-5%'],
                fontSize: 30
            },
            detail: {
                fontSize: 100,
                offsetCenter: [0, '-30%'],
                valueAnimation: true,
                formatter: function (value) {
                    return Math.round(value * 100) + '';
                },
                color: 'inherit'
            },
            data: [
                {
                    value: 0.98,
                    name: 'ACURACIDADE DO ESTOQUE'
                }
            ]
        }
    ]
};
if (option && typeof option === 'object') {
    myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);