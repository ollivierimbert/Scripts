// Copyright QUANTOWER LLC. © 2017-2025. All rights reserved.
//VWAP Indicator

using System;
using TradingPlatform.BusinessLayer;
using System.Drawing;

namespace VWAP
{
    public class VWAP : Indicator
    {
        [InputParameter("VWAP Line Color", 0)]
        public Color vwapColor = Color.Orange;

        private double cumulativeTPV; // Typical Price * Volume
        private double cumulativeVolume;

        public VWAP()
            : base()
        {
            this.Name = "VWAP";
            this.Description = "Volume Weighted Average Price";
            this.SeparateWindow = false;
            this.AddLineSeries("VWAP", this.vwapColor, 2, LineStyle.Solid);
        }

        protected override void OnInit()
        {
            base.OnInit();
            this.cumulativeTPV = 0.0;
            this.cumulativeVolume = 0.0;
        }

        protected override void OnUpdate(UpdateArgs args)
        {
            int bar = this.Count - 1;

            if (bar == 0)
            {
                this.cumulativeTPV = 0.0;
                this.cumulativeVolume = 0.0;
            }

            double typicalPrice = (this.Open() + this.High() + this.Low() + this.Close()) / 4.0;
            double volume = this.Volume();
            this.cumulativeTPV += typicalPrice * volume;
            this.cumulativeVolume += volume;

            double vwap = (this.cumulativeVolume != 0) ? (this.cumulativeTPV / this.cumulativeVolume) : typicalPrice;
            this.SetValue(vwap);
        }
    }
}
