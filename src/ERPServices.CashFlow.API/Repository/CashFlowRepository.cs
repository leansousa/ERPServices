using AutoMapper;
using ERPServices.CashFlow.API.Data.ValueObjects;
using ERPServices.CashFlow.API.Model;
using ERPServices.CashFlow.API.Model.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ERPServices.CashFlow.API.Repository
{
    public class CashFlowRepository : ICashFlowRepository
    {
        private readonly MySQLContext _context;
        private readonly IMapper _mapper;

        public CashFlowRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CashFlowVO>> FindAll()
        {
            List<CashFlowEntity> cashFlows = await _context.CashFlows.ToListAsync();
            return _mapper.Map<List<CashFlowVO>>(cashFlows);
        }
        public async Task<CashFlowVO> FindbyId(long id)
        {
            CashFlowEntity cashFlow = await _context.CashFlows.Where(x => x.Id == id).FirstOrDefaultAsync() ?? new CashFlowEntity();
            return _mapper.Map<CashFlowVO>(cashFlow) ;
        }
        public async Task<CashFlowVO> Create(CashFlowVO vo)
        {
            CashFlowEntity cashFlow = _mapper.Map<CashFlowEntity>(vo);
            _context.CashFlows.Add(cashFlow);
            await _context.SaveChangesAsync();
            return _mapper.Map<CashFlowVO>(cashFlow);
        }
        public async Task<CashFlowVO> Update(CashFlowVO vo)
        {
            CashFlowEntity cashFlow = _mapper.Map<CashFlowEntity>(vo);
            _context.CashFlows.Update(cashFlow);
            await _context.SaveChangesAsync();
            return _mapper.Map<CashFlowVO>(cashFlow);
        }
        public async Task<bool> Delete(long id)
        {
            try
            {
                CashFlowEntity cashFlow = await _context.CashFlows.Where(x => x.Id == id).FirstOrDefaultAsync() ?? new CashFlowEntity();
                if (cashFlow.Id <= 0) return false;
                _context.CashFlows.Remove(cashFlow);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
