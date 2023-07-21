
var dom = document.getElementById('echart--gauge-gradeStockDivergence');
var myChart = echarts.init(dom, null, {
    renderer: 'canvas',
    useDirtyRect: false
});
var frontValue = parseFloat(dom.getAttribute('data-value'));

var app = {};

var option;
option = {
    series: [
        {
            type: 'gauge',
            startAngle: 180,
            endAngle: 0,
            center: ['50%', '90%'],
            radius: '170%',
            min: 0,
            max: 1,
            splitNumber: 8,
            axisLine: {
                lineStyle: {
                    width: 20,
                    color: [
                        [0.05, '#7CFFB2'],
                        [0.10, '#58D9F9'],
                        [0.15, '#FDDD60'],
                        [1, '#FF6E76']
                    ]
                }
            },
            pointer: {
                icon: 'path://M12.8,0.7l12,40.1H0.7L12.8,0.7z',
                length: '22%',
                width: 12,
                offsetCenter: [0, '-55%'],
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
                offsetCenter: [0, '5%'],
                fontSize: 15,
                fontWeight: 'bold'
            },
            detail: {
                fontSize: 55,
                offsetCenter: [0, '-5%'],
                valueAnimation: true,
                formatter: function (value) {
                    return (value * 100).toFixed(1) + '%';
                },
                color: '#012987'
            },
            data: [
                {
                    value: frontValue,
                    name: ''
                }
            ]
        }
    ]
};
if (option && typeof option === 'object') {
    myChart.setOption(option);
}


function resizeChart() {
    myChart.resize();
}

// Adicione o evento de redimensionamento
window.addEventListener('resize', resizeChart);

// Atualize o gráfico no carregamento da página
resizeChart();