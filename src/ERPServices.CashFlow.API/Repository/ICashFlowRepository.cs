using ERPServices.CashFlow.API.Data.ValueObjects;

namespace ERPServices.CashFlow.API.Repository
{
    public interface ICashFlowRepository
    {
        Task<IEnumerable<CashFlowVO>> FindAll();
        Task<CashFlowVO> FindbyId(long id);
        Task<CashFlowVO> Create(CashFlowVO vo);
        Task<CashFlowVO> Update(CashFlowVO vo);
        Task<bool> Delete(long id);
    }
}
