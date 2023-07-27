var dom = document.getElementById('echart--effectiveness');
var myChart = echarts.init(dom, null, {
    renderer: 'canvas',
    useDirtyRect: false
});
var app = {};

var frontValue1 = parseFloat(dom.getAttribute('data-value-1'));
var frontValue2 = parseFloat(dom.getAttribute('data-value-2'));
var frontValue3 = parseFloat(dom.getAttribute('data-value-3'));
var frontValue4 = parseFloat(dom.getAttribute('data-value-4'));

var option;

option = {
    radar: {
        radius: '110%',
        center: ['50%', '60%'],
        indicator: [
            { name: 'Acuracidade de Estoque', max: 100, color: '#012987', fontSize: '22' },
            { name: 'Aproveitamento de Estoque', max: 100, color: '#012987' },
            { name: 'Assertividade das Locações', max: 100, color: '#012987' }
        ]
    },
    series: [
        {
            name: 'Budget vs spending',
            type: 'radar',
            data: [
                {
                    value: [
                        (frontValue1 * 100).toFixed(1),
                        (frontValue2 * 100).toFixed(1),
                        (frontValue3 * 100).toFixed(1)
                    ],
                    name: 'Allocated Budget',
                    label: {
                        show: true,
                        formatter: function (params) {
                            return params.value + '%'; // Adicionar o símbolo de % ao valor
                        }
                    },
                    areaStyle: {
                        color: new echarts.graphic.RadialGradient(0.1, 0.6, 1, [
                            {
                                color: 'rgba(255, 145, 124, 0.1)',
                                offset: 0
                            },
                            {
                                color: 'rgba(255, 145, 124, 0.9)',
                                offset: 1
                            }
                        ])
                    }
                }
            ]
        }
    ],
    graphic: [
        {
            type: 'text',
            left: 'center',
            top: 'middle',
            z: 100, // Move o texto para a frente de todos os elementos
            style: {
                text: (frontValue4 * 100).toFixed(1) + '%',
                fontSize: 40,
                fontWeight: 'bold',
                color: '#012987'
            }
        }
    ],
};


if (option && typeof option === 'object') {
    myChart.setOption(option);
}

window.addEventListener('resize', myChart.resize);